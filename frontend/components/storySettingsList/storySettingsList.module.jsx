"use client";
import Cookies from "js-cookie";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import styles from "./storySettingsList.module.css";

export default function StorySettingsList() {
    const router = useRouter();
    const [isLogged, setIsLogged] = useState(false);

    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);

    return (
        <>
            {isLogged && <div className={styles.list}>
                <div onClick={() => {router.push("/story")}} className={`${styles.item}`}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.add_icon}></div>
                        <p>Add to your story</p>
                    </div>
                </div>
                <div className={`${styles.item}`}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.archive_icon}></div>
                        <p>Stories archive</p>
                    </div>
                </div>
                <div className={`${styles.item}`}>
                    <div className={styles.icon_wrapper} style={{backgroundColor: "rgba(255, 51, 102, 1)"}}>
                        <div className={styles.remove_icon}></div>
                        <p>Remove story</p>
                    </div>
                </div>
            </div>}
        </>
    );
}
