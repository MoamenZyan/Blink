"use client"
import styles from "./page.module.css";
import GetAllUsers from "@/ApiHelper/users/getAllUsers";
import { useEffect, useState } from "react";
import Image from "next/image";
import { formatDistanceToNow } from 'date-fns';
import { useRouter } from "next/navigation";
import Header from "@/components/header/header.module";
import GetUserByUsername from "@/ApiHelper/users/getUserByUsername";
import BrowseStory from "@/components/browseStory/browseStory.module";


export default function StoryPage(props) {
    const router = useRouter();
    const [users, setUsers] = useState([]);
    const [stories, setStories] = useState([]);
    const [prev, setPrev] = useState(null);
    const [current, setCurrent] = useState(null);
    const [currentUser, setCurrentUser] = useState(null);
    const [myStory, setMyStory] = useState(null);
    const [next, setNext] = useState(null);
    const [index, setIndex] = useState(-1);
    const [storyIndex, setStoryIndex] = useState(0);
    const [progress, setProgress] = useState(0);
    const [originalStory, setOriginalStory] = useState([]);
    const [isInterval, setIsInterval] = useState(true);
    const [isPaused, setIsPaused] = useState(false);
    const [intervalId, setIntervalId] = useState(null);

    useEffect(() => {
        const getInfo = async () => {
            const users = await GetAllUsers();
            const user = await GetUserByUsername(localStorage.getItem("userName"));
            if (users.status) {
                setUsers(users.users.$values.filter((user) => user.id.toString() != localStorage.getItem('userId')));
                setCurrentUser(user.user)
                setMyStory(user.user.stories.$values)
                const stories = users.users.$values.filter(user => user.id.toString() != localStorage.getItem('userId')).map((user) => user.stories.$values);
                setStories(stories);
                setStories(stories.map((story, index) => [story, index]))
                setCurrent(users.users.$values.filter((user) => user.id == props.params.user_id)[0].stories.$values);
                const currentLocal = users.users.$values.filter((user) => user.id == props.params.user_id)[0].stories.$values;
                const storiesLocal = stories.map((story, index) => [story, index]);
                setIndex(storiesLocal.filter((story) => story[0] == currentLocal)[0][1]);
                setOriginalStory(users.users.$values.filter((user) => user.id == props.params.user_id)[0].stories.$values);
                setPrev(stories[index - 1] || null);
                setNext(stories[index] || null);
            }
        }
        getInfo();
    }, []);

    useEffect(() => {
        if (!isPaused && isInterval && stories[index + 1]) {
            const id = setInterval(() => {
                setProgress(prevProgress => {
                    if (prevProgress < 100) {
                        return prevProgress + 1;
                    } else {
                        clearInterval(id);
                        setProgress(0);
                        return prevProgress;
                    }
                });
            }, 45);
            setIntervalId(id);
        }

        return () => {
            if (intervalId) {
                clearInterval(intervalId);
            }
        };
    }, [isPaused, isInterval, index]);

    useEffect(() => {
        if (isPaused && intervalId) {
            clearInterval(intervalId);
            setIntervalId(null);
        }
    }, [isPaused]);

    const handleStoryNavigation = (type) => {
        if (type === "next") {
            if (current.length > 1 && current.length - 1 > storyIndex) {
                setStoryIndex(prevIndex => prevIndex + 1);
            } else {
                if (stories[index + 1]) {
                    if (stories[index + 1][0][0] == current[0]) {
                        console.log(stories[index + 2][0]);
                        setCurrent(stories[index + 2][0]);
                        setIndex(prevIndex => prevIndex + 2);
                    } else {
                        setCurrent(stories[index + 1][0]);
                        setIndex(prevIndex => prevIndex + 1);
                    }
                    setStoryIndex(0);
                }
            }
        } else if (type === "prev") {
            if (current.length > 1 && storyIndex > 0) {
                setStoryIndex(prevIndex => prevIndex - 1);
            } else {
                if (index > 0) {
                    setStoryIndex(0);
                    setCurrent(stories[index - 1][0]);
                    setIndex(prevIndex => prevIndex - 1);
                } else {
                    setCurrent(myStory);
                    setIndex(-1);
                }
            }
        }
        setProgress(0);
    };

    useEffect(() => {
        if (progress == 100 && isInterval && !isPaused) {
            handleStoryNavigation("next");
        }
    }, [progress, next])

    useEffect(() => {
        const handleInfo = () => {
            if (stories.length > 0) {
                setPrev(stories[index - 1] || null);
                setNext(stories[index] || null);
            }
        }
        handleInfo();
    }, [index])

    if (prev || current || next) {
        return (
            <>
                <div className={styles.parent}>
                    <Header overlay={false}/>
                    <div className={styles.container}>
                        <div className={styles.center}>
                            <div className={styles.left}>
                                <div onClick={() => {router.push("/story")}} className={styles.add}>
                                    <div className={styles.icon_wrapper}>
                                        <Image className={styles.icon} src={"/assets/story_white.svg"} width={50} height={50}/>
                                    </div>
                                    <p>Add to your story</p>
                                </div>
                                <div className={styles.users}>
                                    <h4>Your story</h4>
                                    <div style={{backgroundColor: `${current == myStory ? "rgb(237, 235, 235)" : ""}`}}
                                        onClick={() => {
                                        if (currentUser.stories.$values.length == 0) {
                                            router.push("/story")
                                        } else {
                                            setIndex(-1);
                                            setCurrent(myStory)
                                            setProgress(0);
                                            setStoryIndex(0);
                                        }
                                        }} className={styles.mine}>
                                        <img src={currentUser.photo}/>
                                        <div className={styles.info_details}>
                                            <h3>{currentUser.username}</h3>
                                            <p>{currentUser.stories.$values.length > 0 ? formatDistanceToNow(new Date(currentUser.stories.$values[currentUser.stories.$values.length - 1].createdAt), {addSuffix: true}): "add story !"}</p>
                                        </div>
                                    </div>
                                    <h4>All stories</h4>
                                    {users.map((user) => (
                                        <div style={{backgroundColor: `${current[0].userId == user.id ? "rgb(237, 235, 235)" : ""}`}} onClick={() => {
                                            setCurrent(user.stories.$values);
                                            setProgress(0);
                                            setIndex(stories.filter((story) => story[0][0] == user.stories.$values[0])[0][1]);
                                            }} className={styles.user}>
                                            <img className={styles.user_photo} src={user.photo}/>
                                            <div className={styles.info_details}>
                                                <h3>{user.username}</h3>
                                                <p>{formatDistanceToNow(new Date(user.stories.$values[user.stories.$values.length - 1].createdAt), {addSuffix: true})}</p>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                            <div className={styles.right}>
                                <BrowseStory
                                navigation={handleStoryNavigation}
                                progress={progress}
                                currentIndex={storyIndex}
                                story={current[storyIndex]}
                                prev={(current != myStory) && ((current == originalStory) || current != originalStory)}
                                next={stories[index + 1] != null}
                                isPaused={isPaused}
                                setIsPaused={setIsPaused}
                                length={current.length} />
                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}