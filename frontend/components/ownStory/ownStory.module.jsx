"use client";
import styles from "./ownStory.module.css";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";

export default function OwnStory({user}) {
    const router = useRouter();
    const [stories, setStories] = useState([]);

    return(
        <>
            <div style={{backgroundColor: `${user.stories.$values.length > 0 &&
                                            user.stories.$values[0].photo == "null" && 
                                            user.stories.$values[0].backgroundColor}`}} className={styles.story_div}>
                <div className={styles.stories_lines}>
                    {stories && stories.length > 0 && stories.map(() => (
                        <div className={styles.white_line}></div>
                    ))}
                </div>
                <div className={styles.bodyWrapper}>
                    {user.stories.$values.length > 0 ?
                        <><img className={styles.blured_photo} src={user.stories.$values[0].photo} width={100} height={50}/></> :
                        <Image className={styles.story_photo} src={"/assets/blured_image.png"} alt="" width={170} height={300} />
                    }
                    {user && user.photo == "null" ? <div className={styles.user_default}></div> :
                    <img className={styles.user_photo} src={user && user.photo}/>
                    }
                </div>
                <div onClick={() => {router.push("/story")}} className={styles.add_story}>
                    <div className={styles.star}></div>
                </div>
            </div>
        </>
    );
}
