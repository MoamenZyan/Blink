import styles from "./commentRepliesSection.module.css";
import CommentReply from "./commentReply.module";

export default function RepliesSection({replies, setCommentCount, setReplySection, setTrigger, trigger}) {
    return (
        <>
            <div className={styles.parent}>
                {replies.map((reply) => (
                    <CommentReply setCommentCount={setCommentCount} setTrigger={setTrigger} trigger={trigger} reply={reply}/>
                ))}
                <p onClick={() => {setReplySection(false)}} className={styles.hide}>hide replies</p>
            </div>
        </>
    )
}