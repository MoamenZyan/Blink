"use client";
import Image from "next/image";
import styles from "./createPost.module.css";
import EmojiPicker from "emoji-picker-react";
import { useState, useRef } from "react";
import CreatePost from "@/ApiHelper/posts/createPost";
import CreatePostLoader from "./createPostLoader.module";



export default function CreatePostPopup({setPopup, trigger, setTrigger}) {
    const input = useRef(null);
    const [caption, setCaption] = useState(null);
    const [photoURL, setPhotoURL] = useState(null);
    const [photo, setPhoto] = useState(null);
    const [picker, setPicker] = useState(false);
    const [privacy, setPrivacy] = useState(null);
    const [uploading, setUploading] = useState(false);
    
    const handleInputClick = () => {
        input.current.click();
    }
    
    const handlePhotoUpload = () => {
        const Photo = input.current.files[0];
        if (Photo) {
            setPhoto(Photo);
            setPhotoURL(URL.createObjectURL(Photo));
        }
    }

    const captionOnChange = (event) => {
        const {value} = event.target;
        setCaption(value);
    }

    const handleEmojiClick = (emoji) => {
        setCaption(prevCaption => prevCaption + emoji.emoji);
      };

    const handleCreatePost = async () => {
        const form = new FormData();
        form.append("Caption", caption);
        form.append("Photo", photo);
        form.append("Privacy", privacy);
        setUploading(true);
        const result = await CreatePost(form);
        if (result == true)
        {
            setTrigger(!trigger);
            setUploading(false);
            setPopup(false);
        }
    }

    return (
        <>
            <div className={styles.popup}>
                <h2>Create Post</h2>
                <div className={styles.body}>
                    <div className={styles.left}>
                        <div className={styles.textarea_div}>
                            <textarea value={caption} onChange={captionOnChange} placeholder="Create caption..." />
                            <Image alt="" onClick={() => {setPicker(!picker)}} src={"/assets/smile.svg"} width={30} height={30}
                            style={{position: "absolute", right: "10px", bottom: "10px", cursor: "pointer"}} 
                            />
                        </div>
                        {picker && <EmojiPicker height={400} onEmojiClick={handleEmojiClick} style={{position: "absolute"}}/>}
                        <div className={styles.left_div_flex}>
                            <div className={styles.check_boxes}>
                                <div>
                                    <input type="checkbox" name="" id="hide_likes" />
                                    <label htmlFor="hide_likes">Hide Likes</label>
                                </div>
                                <div>
                                    <input type="checkbox" name="" id="disable_comments" />
                                    <label htmlFor="disable_comments">Disable Commenting</label>
                                </div>
                                <div>
                                    <input type="checkbox" name="" id="disable_share" />
                                    <label htmlFor="disable_share">Disable Sharing</label>
                                </div>
                            </div>
                            <div className={styles.privacy}>
                                <h4>Post Privacy:</h4>
                                <select onChange={(event) => {setPrivacy(event.target.value)}} className={styles.select} name="privacy" id="privacy">
                                    <option value="">-- post privacy --</option>
                                    <option value="public">public</option>
                                    <option value="friends">friends</option>
                                    <option value="private">private</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div className={styles.right}>
                        <div className={styles.photo}>
                            <div style={{position: "relative"}}>
                                <Image priority alt="" onClick={handleInputClick} src={photoURL == null ? "/assets/default.svg" : photoURL} width={180} height={180}
                                style={{cursor: "pointer", objectFit: "cover", borderRadius: "20px"}}/>
                                {photo != null && <Image src={"/assets/exit.svg"} width={30} height={30} alt=""
                                style={{position: "absolute", right: "5px", top: "5px",
                                backgroundColor: "white", borderRadius: "50%", cursor: "pointer"}}
                                onClick={() => {
                                    setPhoto(null);
                                    setPhotoURL(null);
                                }}
                                />}
                            </div>
                            <input onChange={handlePhotoUpload} ref={input} type="file" style={{display: "none", visibility: "hidden"}} />
                        </div>
                        <div onClick={handleCreatePost} className={styles.upload}>
                            <p>Upload Post</p>
                            <Image alt="" src={"/assets/check.svg"} width={20} height={20} />
                        </div>
                    </div>
                </div>
                <Image src={"/assets/exit.svg"} width={50} height={50} alt=""
                    style={{position: "absolute", top: "-15px", right: "-15px",
                        backgroundColor: "white", borderRadius: "50%", cursor: "pointer"}}
                        onClick={() => {setPopup(false)}}
                />
                {uploading && <>
                    <div className={styles.overlay}></div>
                    <CreatePostLoader imageURL={photoURL} />
                </>}
            </div>
        </>
    )
}