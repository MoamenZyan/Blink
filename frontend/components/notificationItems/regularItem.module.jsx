import styles from "./regularItem.module.css";
import { formatDistanceToNow } from "date-fns";

export default function PostNotification({setTrigger, trigger, notification}) {
    return (
        <>
            <div className={styles.item}>
                <div className={styles.profile_photo}><img src={notification.userPhoto} width={60} height={60}/></div>
                <p><strong>{notification.username}</strong> {notification.message}</p>
                <p className={styles.time}>{formatDistanceToNow(new Date(notification.createdAt), {addSuffix: true})}</p>
            </div>
        </>
    );
}
