"use client";
import styles from "./settingsList.module.css";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export default function SettingsList({user}) {
    const [isMine, setIsMine] = useState(false);
    const router = useRouter();

    useEffect(() => {
        if (user.username == localStorage.getItem("userName")) {
            setIsMine(true);
        }
    }, []);

    return(
        <>
            <div className={styles.settings}>
                {isMine && <>
                    <div onClick={() => {router.push("/story")}} className={styles.add}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.add_icon}></div>
                        </div>
                        <p>Add to your story</p>
                    </div>
                    <div className={styles.save}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.save_icon}></div>
                        </div>
                        <p>Saved posts</p>
                    </div>
                    <div className={styles.archive}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.archive_icon}></div>
                        </div>
                        <p>Archive</p>
                    </div>
                    <div onClick={() => {router.push("/profile/settings")}} className={styles.options}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.options_icon}></div>
                        </div>
                        <p>Options & settings</p>
                    </div>
                </>}
                {!isMine && <>
                    <div className={styles.share}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.share_icon}></div>
                    </div>
                    <p>Share this profile</p>
                </div>
                <div className={styles.favorite}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.favorite_icon}></div>
                    </div>
                    <p>Add to favorites</p>
                </div>
                <div className={styles.block}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.block_icon}></div>
                    </div>
                    <p>Block user</p>
                </div>
                </>
                }
            </div>
        </>
    )
}
