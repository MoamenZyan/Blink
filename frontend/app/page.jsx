"use client";
import styles from "./page.module.css";
import Header from "@/components/header/header.module";
import StoriesWrapper from "@/components/storiesWrapper/storiesWrapper.module";
import PostsWrapper from "@/components/postsWrapper/postsWrapper.module";
import UserCard from "@/components/user_card/userCard.module";
import StorySettingsList from "@/components/storySettingsList/storySettingsList.module";
import PostFixedButton from "@/components/postFixedButton/postFixedButton.module";
import GetAllPosts from "@/ApiHelper/posts/getAllPosts";
import { useEffect, useState } from "react";
import GetUserByUsername from "@/ApiHelper/users/getUserByUsername";
import CreatePostPopup from "@/components/createPost/createPost.module";

export default function HomePage() {
    const [createPost, setCreatePost] = useState(false);
    const [posts, setPosts] = useState([]);
    const [trigger, setTrigger] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
        const getInfo = async () => {
            const result = await GetAllPosts();
            const User = await GetUserByUsername(localStorage.getItem('userName'));
            if (result.status && User.status) {
                setUser(User.user);
                setPosts(result.posts.$values);
            }
        };
        getInfo();
    }, [trigger]);

    if (user == null) {
        return (<></>)
    } else {
        return (
            <>
                <div className={styles.parent}>
                    <Header overlay={createPost} />
                    <div className={styles.container}>
                        <div className={styles.center}>
                            <div className={styles.left_div}>
                                <UserCard user={user && user} />
                            </div>
                            <div className={styles.center_div}>
                                <StoriesWrapper user={user && user} />
                                <PostsWrapper posts={posts} />
                            </div>
                            <div className={styles.right_div}>
                                <StorySettingsList />
                            </div>
                        </div>
                        <PostFixedButton setCreatePost={setCreatePost} />
                    </div>
                    {createPost && <div className={styles.overlay}></div>}
                    {createPost && <CreatePostPopup trigger={trigger} setTrigger={setTrigger} setPopup={setCreatePost} />}
                </div>
            </>
        );
    }
}
