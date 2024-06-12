"use client";
import styles from "./post.module.css";
import { useState } from "react";

export default function Post() {
    const [liked, setLiked] = useState(false);
    const like = () => {
        setLiked(!liked);
    }
    return (
        <>
            <div className={styles.post_div}>
                <div className={styles.header}>
                    <div className={styles.info}>
                        <div className={styles.user_photo}></div>
                        <div className={styles.user_info}>
                            <h2>user name</h2>
                            <p>1h ago</p>
                        </div>
                    </div>
                    <div className={styles.settings_wrapper}>
                        <div className={styles.settings}></div>
                    </div>
                </div>
                <div className={styles.body}></div>
                <div className={styles.reactions}>
                    <div className={styles.left}>
                        <div onClick={like} className={styles.like}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.like_icon} style={{backgroundColor: liked && "#FCB341"}}></div>
                            </div>
                            <div className={styles.like_count}>50k Blinks</div>
                        </div>
                        <div className={styles.comment}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.comment_icon}></div>
                            </div>
                            <div className={styles.comment_count}>1.3k Comments</div>
                        </div>
                        <div className={styles.share}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.share_icon}></div>
                            </div>
                            <div className={styles.share_count}>300 Shares</div>
                        </div>
                    </div>
                    <div className={styles.right}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.save}></div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}