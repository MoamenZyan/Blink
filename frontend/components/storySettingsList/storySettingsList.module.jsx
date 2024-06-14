"use client";
import Cookies from "js-cookie";
import { useEffect, useState } from "react";
import styles from "./storySettingsList.module.css";

export default function StorySettingsList() {

    const [isLogged, setIsLogged] = useState(false);
    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);

    return (
        <>
            {isLogged && <div className={styles.list}>
                <div className={`${styles.item}`}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.add_icon}></div>
                    </div>
                    <p>Add to your story</p>
                </div>
                <div className={`${styles.item}`}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.archive_icon}></div>
                    </div>
                    <p>Stories archive</p>
                </div>
                <div className={`${styles.item}`}>
                    <div className={styles.icon_wrapper} style={{backgroundColor: "rgba(255, 51, 102, 1)"}}>
                        <div className={styles.remove_icon}></div>
                    </div>
                    <p>Remove story</p>
                </div>
            </div>}
        </>
    );
}
