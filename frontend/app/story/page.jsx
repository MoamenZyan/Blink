"use client";
import styles from "./page.module.css";
import Header from "@/components/header/header.module";
import Image from "next/image";
import { useState, useRef } from "react";
import EmojiPicker from "emoji-picker-react";
import CreateStory from "@/ApiHelper/stories/createStory";
import StoryUpload from "@/components/story/storyUpload.module";
import { useRouter } from "next/navigation";

export default function StoryPage() {
    const router = useRouter();
    const textColorInput = useRef(null);
    const backgroundColorInput = useRef(null);
    const photoInput = useRef(null);
    const [storyText, setStoryText] = useState("");
    const [emojiPicker, setEmojiPicker] = useState(false);
    const [photo, setPhoto] = useState(null);
    const [photoURL, setPhotoURL] = useState(null);
    const [text, setText] = useState(false);
    const [backgroundColor, setBackgroundColor] = useState("white");
    const [textColor, setTextColor] = useState("black");
    const [uploading, setUploading] = useState(false);

    const photoSelection = () => {
        const Photo = photoInput.current.files[0];
        if (Photo) {
            setPhoto(Photo);
            setPhotoURL(URL.createObjectURL(Photo));
        }
    }

    const handleStoryCreation = async () => {
        const form = new FormData();
        form.append("Photo", photo);
        if (text) {
            form.append("Content", storyText);
        } else {
            form.append("Content", null);
        }
        form.append("Privacy", "public");
        form.append("backgroundColor", backgroundColor);
        form.append("textColor", textColor);
        setUploading(true);
        await CreateStory(form);
        setUploading(false);
        router.push("/");
    }

    return (
        <>
            <div className={styles.parent}>
                <Header overlay={uploading}/>
                <div className={styles.container}>
                    <h2>Create Your Story</h2>
                    <div className={styles.content}>
                        <div className={styles.left}>
                            <div onClick={handleStoryCreation} className={styles.wrapper}>
                                <div className={`${styles.upload} ${styles.button}`}>
                                    <Image className={styles.icon} src={"/assets/upload.svg"} width={40} height={40} alt="" />
                                    <p>Upload Story</p>
                                </div>
                            </div>
                            <div className={styles.wrapper}>
                                <div className={`${styles.upload} ${styles.button}`}>
                                    <Image className={styles.icon} src={"/assets/public_white.svg"} width={40} height={40} alt=""/>
                                    <p>Public</p>
                                </div>
                            </div>
                        </div>
                        <div style={{backgroundColor: `${backgroundColor}`}} className={styles.story_div}>
                            {text && <textarea onChange={(e) => {setStoryText(e.target.value)}} value={storyText} placeholder="Enter Text..." style={{color: `${textColor}`}} className={styles.text}/>}
                            {text && <div onClick={() => {setEmojiPicker(!emojiPicker)}} className={styles.emoji}>
                                <Image src={"/assets/smile.svg"} width={50} height={50}/>
                            </div>}
                            {photo && <div className={styles.story_photo_div}><Image className={styles.story_photo} src={photoURL} width={10} height={100} alt=""/></div>}
                        </div>
                        <div className={styles.right}>
                                <div onClick={() => {setText(!text)}} className={`${styles.normal}`}>
                                    <div className={styles.icon}>
                                        <Image className={styles.icon} src={"/assets/text.svg"} width={40} height={40} alt="" />
                                    </div>
                                    <p>{text == false ? "Add" : "Remove"} Text</p>
                                </div>
                                <div className={`${styles.normal}`}>
                                    <div className={styles.icon}>
                                        <Image className={styles.icon} src={"/assets/mention.svg"} width={40} height={40} alt="" />
                                    </div>
                                    <p>Mention</p>
                                </div>
                                <div onClick={() => {backgroundColorInput.current.click()}} className={`${styles.normal}`}>
                                    <div className={styles.icon}>
                                        <Image className={styles.icon} src={"/assets/color.svg"} width={40} height={40} alt="" />
                                    </div>
                                    <div className={styles.background_color_box_wrapper}>
                                        <input style={{visibility: "hidden"}} ref={backgroundColorInput} type="color" value={textColor} onChange={(e) => {setBackgroundColor(e.target.value)}} />
                                    </div>
                                    <p>Background Color</p>
                                </div>
                                <div onClick={() => {textColorInput.current.click()}} className={`${styles.normal}`}>
                                    <div className={styles.icon}>
                                        <Image className={styles.icon} src={"/assets/text_color.svg"} width={40} height={40} alt="" />
                                    </div>
                                    <div className={styles.text_color_box_wrapper}>
                                        <input style={{visibility: "hidden"}} ref={textColorInput} type="color" value={textColor} onChange={(e) => {setTextColor(e.target.value)}} />
                                    </div>
                                    <p>Text Color</p>
                                </div>
                                <div onClick={() => {photo == null ? photoInput.current.click() : 
                                    setPhoto(null);
                                    setPhotoURL(null);
                                }} className={`${styles.normal}`}>
                                    <div className={styles.icon}>
                                        <Image className={styles.icon} src={"/assets/photo_white.svg"} width={30} height={30} alt="" />
                                    </div>
                                    <input onChange={photoSelection} ref={photoInput} type="file" style={{display: "none", visibility: "hidden"}}/>
                                    <p>{photo == null ? "Add" : "Remove"} Photo</p>
                                </div>
                        </div>
                    </div>
                </div>
                {emojiPicker && text && <div className={styles.emoji_div}>
                    <EmojiPicker onEmojiClick={(emoji) => {setStoryText(text => text + emoji.emoji)}} />
                </div>}
                {uploading && <>
                    <div className={styles.overlay}></div>
                    <StoryUpload backgroundColor={backgroundColor} text={storyText} imageURL={photoURL} textColor={textColor} />
                </>}
            </div>
        </>
    )
}
