"use client";
import { useState } from "react";
import Image from "next/image";
import { useRouter } from "next/navigation";
import styles from "./friend.module.css";
import SendFriendRequest from "@/ApiHelper/friendRequests/sendFriendRequestAPI";
import DeleteFriend from "@/ApiHelper/friendRequests/deleteFriendAPI";
import Spinner from "@/components/spinner/spinner.module";


export default function Friend({user, setTrigger, trigger}) {
    const router = useRouter();
    const [friendStatus, setFriendStatus] = useState(user.friendStatus);
    const [loading, setLoading] = useState(false);


    const handleFriendRequest = async (id) => {
        setLoading(true);
        const result = await SendFriendRequest(id);
        setFriendStatus("pending");
        setLoading(false);
    }

    const handleDeleteFriendRequest = async (id) => {
        setLoading(true);
        const result = await DeleteFriend(id);
        setFriendStatus("no friend request");
        setLoading(false);
    }


    return (
        <>
            <div className={styles.user}>
                <div onClick={() => {router.push(`/profile/${user.username}`)}} className={styles.photo_div}><img src={user.photo} width={50} height={50}/></div>
                <div className={styles.user_info}>
                    <h2 onClick={() => {router.push(`/profile/${user.username}`)}}>@{user.username}</h2>
                    <p>{user.country}, {user.city}</p>
                </div>
                {friendStatus != "pending" && <div className={styles.button}>{!loading ? <button onClick={() => {handleFriendRequest(user.id)}}>Send Friend Request</button> :
                <Spinner width={35} height={35} color={"white"} border={5}/>}</div>}
                {friendStatus == "pending" && 
                    <div onClick={() => {handleDeleteFriendRequest(user.id)}} className={styles.pending}>
                        <button>Pending Request</button>
                        <Image className={styles.icon} src={"/assets/time.svg"} width={10} height={10} alt=""/>
                    </div>
                }
            </div>
        </>
    )
}
