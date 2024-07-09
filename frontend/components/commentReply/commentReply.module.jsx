"use client";
import { useEffect, useState } from "react";
import styles from "./commentReply.module.css";
import { formatDistanceToNowStrict } from "date-fns";
import AddCommentReply from "./addCommentReply.module";
import DeleteReply from "@/ApiHelper/commentsReplies/deleteReplyAPI";
export default function CommentReply({reply, setTrigger, trigger, setCommentCount}) {
    const [isReply, setIsReply] = useState(false);
    const [mine, setMine] = useState(false);
    useEffect(() => {
        if (reply.username == localStorage.getItem('userName')) {
            setMine(true);
        }
    }, []);

    const handleDelete = async () => {
        const result = await DeleteReply(Number(reply.id));
        if (result) {
            setCommentCount(count => count - 1);
            setTrigger(!trigger);
        }
    }

    return (
        <>
            <div className={styles.parent}>
                <div className={styles.info}>
                    <div><img className={styles.profile_photo} src={reply.userPhoto}/></div>
                    <div className={styles.core}>
                        <h3>@{reply.username}</h3>
                        <p>{reply.content}</p>
                    </div>
                </div>
                <span className={styles.line}></span>
                <div className={styles.reactions}>
                    <div className={styles.left}>
                        <button onClick={() => {setIsReply(!isReply)}}>Reply</button>
                        <button>Like</button>
                    </div>
                    <div className={styles.right}>
                        <button>Report</button>
                        {mine && <button onClick={handleDelete}>Delete</button>}
                    </div>
                </div>
                <p className={styles.time}>{formatDistanceToNowStrict(new Date(reply.createdAt), {addSuffix: true})}</p>
            </div>
            {isReply && <AddCommentReply setCommentCount={setCommentCount} setTrigger={setTrigger} trigger={trigger} postId={reply.postId} commentId={reply.commentId} />}
        </>
    )
}