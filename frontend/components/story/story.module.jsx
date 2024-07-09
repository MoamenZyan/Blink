"use client";
import styles from "./story.module.css";
import { useRouter } from "next/navigation";
import { formatDistanceToNow } from 'date-fns';

export default function Story({user}) {
    const router = useRouter();
    return (
        <>
            <div onClick={() => {router.push(`/stories/${user.stories.$values[0].userId}`)}}
             style={{backgroundColor: `${user.stories.$values[0].backgroundColor}`}} className={styles.story_div}>
                <div className={styles.stories_lines}>
                    {user.stories.$values.length > 1 && user.stories.$values.map(() => (
                        <div className={styles.white_line}></div>
                    ))}
                </div>
                <div  className={styles.body}>
                    {user.stories.$values[0].photo != "null" && <div className={styles.body_photo}>
                        <img src={user.stories.$values[0].photo}/>
                    </div>}
                    <p className={styles.story_content} style={{color: `${user.stories.$values[0].textColor}`}}>{user.stories.$values[0].content}</p>
                </div>
                <div className={styles.info}>
                    {user.photo == "null" ? <div className={styles.photo}></div>:
                    <img src={user.photo} className={styles.user_photo}/>
                    }
                    <div className={styles.user_info}>
                        <h3>{user.username}</h3>
                        <p className={styles.created_at}>{formatDistanceToNow(new Date(user.stories.$values[0].createdAt), {addSuffix: true})}</p>
                    </div>
                </div>
            </div>
        </>
    );
}
