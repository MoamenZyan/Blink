"use client";
import styles from "./comment.module.css";
import { formatDistanceToNow } from 'date-fns';
import CommentReply from "../commentReply/addCommentReply.module";
import { useEffect, useState } from "react";
import RepliesSection from "../commentReply/commentRepliesSection.module";
import DeleteCommentAPI from "@/ApiHelper/comments/deleteCommentAPI";

export default function PostComment({comment, setCommentCount, setTrigger, trigger, postId}) {
    const [reply, setReply] = useState(false);
    const [replySection, setReplySection] = useState(false);
    const [mine, setMine] = useState(false);

    useEffect(() => {
        if (comment.user.username == localStorage.getItem('userName')) {
            setMine(true);
        }
    }, []);

    const handleDelete = async () => {
        await DeleteCommentAPI(comment.id);
        setTrigger(!trigger);
        setCommentCount(count => count - 1);
    }

    return (
    <>
        <div className={styles.parent}>
            <div className={styles.core}>
                <div><img className={styles.user_profile_photo} src={comment.user.photo} alt=""/></div>
                <div className={styles.content}>
                    <h3>@{comment.user.username}</h3>
                    <p>{comment.content}</p>
                </div>
            </div>
            <div className={styles.time}>{formatDistanceToNow(new Date(comment.createdAt), {addSuffix: true})}</div>
            <span className={styles.line}></span>
            <div className={styles.comment_reactions}>
                <div>
                    <button onClick={() => {setReply(!reply)}}>Reply</button>
                    <button>Like</button>
                </div>
                <div>
                    <button>Report</button>
                    {mine && <button onClick={handleDelete}>Delete</button>}
                </div>
            </div>
        </div>
        {comment.replies.$values.length > 0 && replySection == false && <div onClick={() => {setReplySection(true)}} className={styles.replies_continue}>
            <div className={styles.continue}></div>
            <p>{comment.replies.$values.length} replies</p>
        </div>}
        {replySection && <RepliesSection setTrigger={setTrigger} trigger={trigger} setCommentCount={setCommentCount} setReplySection={setReplySection} replies={comment.replies.$values}/>}
        {reply && <CommentReply commentId={comment.id} setCommentCount={setCommentCount} setTrigger={setTrigger} trigger={trigger} postId={postId}/>}
    </>
    );
}