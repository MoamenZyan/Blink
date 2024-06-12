import Cookies from "js-cookie";
import styles from "./page.module.css";
import Header from "@/components/header/header.module";
import Post from "@/components/post/post.module";
import StoriesWrapper from "@/components/storiesWrapper/storiesWrapper.module";
import PostsWrapper from "@/components/postsWrapper/postsWrapper.module";
import UserCard from "@/components/user_card/userCard.module";
export default function HomePage() {
    return (
        <>
            <Header />
            <div className={styles.container}>
                <div className={styles.center}>
                    <div className={styles.left_div}>
                        <UserCard />
                    </div>
                    <div className={styles.center_div}>
                        <StoriesWrapper />
                        <PostsWrapper />
                    </div>
                    <div className={styles.right_div}></div>
                </div>
            </div>
        </>
    );
}
