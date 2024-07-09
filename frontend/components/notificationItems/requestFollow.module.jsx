"use client";
import styles from "./requestFollow.module.css";
import { formatDistanceToNow } from "date-fns";
import { useRouter } from "next/navigation";
import AcceptFriendRequest from "@/ApiHelper/friendRequests/acceptFriendRequestAPI";
import RejectFriendRequest from "@/ApiHelper/friendRequests/rejectFriendRequestAPI";

export default function RequestFollow({notification, setTrigger, trigger}) {
    const router = useRouter();

    const handleAcceptRequest = async () => {
        await AcceptFriendRequest(notification.userId);
        setTrigger(!trigger);
    }

    const handleRejectRequest = async () => {
        await RejectFriendRequest(notification.userId);
        setTrigger(!trigger);
    }

    return (
        <>
            <div className={styles.item}>
                <div className={styles.header}>
                    <h3>FOLLOW REQUEST</h3>
                    <p className={styles.time}>{formatDistanceToNow(new Date(notification.createdAt), {addSuffix: true})}</p>
                </div>
                <div onClick={() => {router.push(`/profile/${notification.username}`)}} className={styles.body}>
                    {notification.userPhoto == "null" ? <div className={styles.photo}></div> :
                        <img className={styles.userPhoto} src={notification.userPhoto} width={50} height={50} alt=""/>
                    }
                    <div className={styles.info}>
                        <h3>{notification.username}</h3>
                        <p className={styles.email}>{notification.email}</p>
                        <p className={styles.address}>{notification.country}, {notification.city}</p>
                    </div>
                </div>
                <div className={styles.options}>
                    <button className={styles.reject} onClick={handleRejectRequest}>REJECT</button>
                    <button className={styles.accept} onClick={handleAcceptRequest}>ACCEPT</button>
                </div>
            </div>
        </>
    );
}
