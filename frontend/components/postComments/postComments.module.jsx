"use client";
import { useEffect, useRef } from "react";
import AddComment from "./addComment.module";
import PostComment from "./comment.module";
import styles from "./postComments.module.css";


export default function PostCommentsSection({postId, comments, trigger, setTrigger, setCommentCount, isComments}) {
    const parent = useRef(null);
    const center = useRef(null);

    useEffect(() => {
        console.log("hello")
        if (isComments) {
            parent.current.classList.add(styles.show);
            parent.current.classList.remove(styles.parent);
            center.current.classList.add(styles.center_show);
            center.current.classList.remove(styles.center);
        } else {
            parent.current.classList.remove(styles.show);
            parent.current.classList.add(styles.parent);
            center.current.classList.remove(styles.center_show);
            center.current.classList.add(styles.center);
        }
    }, [isComments]);

    return (
    <>
        <div ref={parent} className={styles.parent}>
            <div ref={center} className={styles.center}>
                {isComments && <>
                {comments.length > 0 && <div className={styles.comments}>
                    {comments.slice(0, 2).map((comment) => (
                        <PostComment postId={postId} setCommentCount={setCommentCount} setTrigger={setTrigger} trigger={trigger} key={comment.id} comment={comment}/>
                    ))}
                </div>}
                {comments.length == 0 && <p className={styles.no_comment}>Be the first to comment !</p>}
                {comments.length > 2 && <div className={styles.other_comments}>{comments.length - 2} other comments</div>}
                <AddComment setCommentsCount={setCommentCount} setTrigger={setTrigger} trigger={trigger} postId={postId} />
                </>}
            </div>
        </div>
    </>
    );
}
