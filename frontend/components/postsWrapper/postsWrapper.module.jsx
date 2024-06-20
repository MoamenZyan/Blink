import styles from "./postsWrapper.module.css";
import Post from "../post/post.module";
import ProfilePhotoPost from "../profilePhotoPost/profilePhotoPost.module";

export default function PostsWrapper({posts}) {
    return (
        <>
            <div className={styles.posts}>
                {posts.length > 0 && posts.map((post) => (
                    post.type == "profile_photo" ? (
                        <ProfilePhotoPost key={post.id} post={post}/>
                    ) : (
                        <Post key={post.id} post={post}/>
                    )
                ))}
            </div>
        </>
    )
}
