"use client";
import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import styles from "./storiesWrapper.module.css";
import Story from "../story/story.module";
import OwnStory from "../ownStory/ownStory.module";
import GetAllStories from "@/ApiHelper/stories/getAllStories";
import GetAllUsers from "@/ApiHelper/users/getAllUsers";

export default function StoriesWrapper({user}) {
    const [isLogged, setIsLogged] = useState(false);
    const [stories, setStories] = useState([]);
    const [users, setUsers] = useState([]);
    const [currentUser, setCurrentUser] = useState(localStorage.getItem('userId'));

    useEffect(() => {
        const getInfo = async () => {
            var stories = await GetAllStories();
            var users = await GetAllUsers();
            if (stories.status && users.status) {
                setStories(stories.stories.$values);
                setUsers(users.users.$values);
            }
        }
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
        getInfo();
    }, []);

    return (
        <>
            <div className={styles.story_div}>
                {isLogged && <OwnStory user={user} />}
                {users.length > 0 && users.map((user) => (
                    user.username !== localStorage.getItem('userName') && user.stories.$values.length > 0 && <Story user={user} />
                ))}
            </div>
        </>
    )
}
