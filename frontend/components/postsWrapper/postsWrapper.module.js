import styles from "./postsWrapper.module.css";
import Post from "../post/post.module";
export default function PostsWrapper() {
    return (
        <>
            <div className={styles.posts}>
                <Post />
                <Post />
                <Post />
            </div>
        </>
    )
}
