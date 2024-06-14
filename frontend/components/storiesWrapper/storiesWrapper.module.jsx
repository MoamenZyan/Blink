"use client";
import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import styles from "./storiesWrapper.module.css";
import Story from "../story/story.module";
import OwnStory from "../ownStory/ownStory.module";

export default function StoriesWrapper(props) {
    const [isLogged, setIsLogged] = useState(false);
    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);
    return (
        <>
            <div className={styles.story_div}>
                {isLogged && <OwnStory />}
                <Story />
                <Story />
                <Story />
                <Story />
                <Story />
                <Story />
            </div>
        </>
    )
}
