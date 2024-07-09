import styles from "./postsWrapper.module.css";
import Post from "../post/post.module";
import ProfilePhotoPost from "../profilePhotoPost/profilePhotoPost.module";

export default function PostsWrapper({posts, setTrigger, trigger}) {
    return (
        <>
            <div className={styles.posts}>
                {posts.length > 0 && posts.map((post) => (
                    post.type == "profile_photo" ? (
                        <ProfilePhotoPost setTrigger={setTrigger} trigger={trigger} key={post.id} post={post}/>
                    ) : (
                        <Post setTrigger={setTrigger} trigger={trigger} key={post.id} post={post}/>
                    )
                ))}
            </div>
        </>
    )
}
