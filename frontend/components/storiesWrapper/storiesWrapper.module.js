
import styles from "./storiesWrapper.module.css";
import Story from "../story/story.module";
export default function StoriesWrapper() {
    return (
        <>
            <div className={styles.story_div}>
                <Story />
                <Story />
                <Story />
                <Story />
                <Story />
                <Story />
            </div>
        </>
    )
}
