import AddComment from "../postComments/addComment.module";
import styles from "./addCommentReply.module.css"


export default function AddCommentReply({postId, setTrigger, trigger, setCommentCount, commentId}) {
    return (
        <>
            <div className={styles.parent}>
                <span></span>
                <AddComment
                    commentId={commentId}
                    type={"reply"}
                    postId={postId}
                    setTrigger={setTrigger}
                    trigger={trigger}
                    setCommentsCount={setCommentCount} />
            </div>
        </>
    )
}