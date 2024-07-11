"use client";
import { useRouter } from "next/navigation";
import styles from "./userFriends.module.css";


export default function UserFriends({friends}) {
    const router = useRouter();
    return (
        <>
            <div className={styles.parent}>
                {friends[0] && <img onClick={() => {router.push(`/profile/${friends[0].friend.username}`)}} className={`${styles.photo} ${styles.photo1}`} src={friends[0].friend.photo} width={50} height={50}/>}
                {friends[1] && <img onClick={() => {router.push(`/profile/${friends[1].friend.username}`)}} className={`${styles.photo} ${styles.photo2}`} src={friends[1].friend.photo} width={50} height={50}/>}
                {friends[2] && <img onClick={() => {router.push(`/profile/${friends[2].friend.username}`)}} className={`${styles.photo} ${styles.photo3}`} src={friends[2].friend.photo} width={50} height={50}/>}
                {friends[3] && <img onClick={() => {router.push(`/profile/${friends[3].friend.username}`)}} className={`${styles.photo} ${styles.photo4}`} src={friends[3].friend.photo} width={50} height={50}/>}
                {friends[4] && <img onClick={() => {router.push(`/profile/${friends[4].friend.username}`)}} className={`${styles.photo} ${styles.photo5}`} src={friends[4].friend.photo} width={50} height={50}/>}
            </div>
        </>
    )
}