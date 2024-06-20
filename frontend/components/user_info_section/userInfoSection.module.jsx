"use client"
import Image from "next/image";
import Spinner from "../spinner/spinner.module";
import ProfilePhotoList from "./profilePhotoList.module";
import styles from "./userInfoSection.module.css";
import UploadUserPhoto from "@/ApiHelper/users/uploadUserPhoto";
import EmojiPicker from "emoji-picker-react";
import { useState, useRef, useEffect } from "react";
import UpdateUserAbout from "@/ApiHelper/users/updateUserAbout";


export default function UserInfoSection({user, isLogged, setTrigger, trigger}) {
    const list = useRef(null);
    const [photoList, setPhotoList] = useState(false);
    const [uploading, setUploading] = useState(false);
    const [editBio, setEditBio] = useState(false);
    const [bio, setBio] = useState(user.about);
    const [editedBio, setEditedBio] = useState(bio);
    const [emojiPicker, setEmojiPicker] = useState(false);
    const [isMine, setIsMine] = useState(false);

    const handleClickOutside = (event) => {
        if (list.current && !list.current.contains(event.target)) {
            setPhotoList(false);
        }
    }

    const handleTextArea = (event) => {
        const {value} = event.target;
        setEditedBio(value);
    }

    const handleEmojiClick = (emoji) => {
        setEditedBio(prevCaption => prevCaption + emoji.emoji);
      };

    const handleUpload = async (input) => {
        const formData = new FormData();
        const photo = input.current.files[0];
        if (photo) {
            formData.append('photo', photo);
            setUploading(true);
            await UploadUserPhoto(formData);
            setUploading(false);
            setTrigger(!trigger);
        }
    }

    const handleUpdateAbout = async (bio) => {
        const form = new FormData();
        form.append("About", bio);
        await UpdateUserAbout(form);
    }



    useEffect(() => {
        const url = window.location.pathname;
        if (url.slice(url.lastIndexOf("/") + 1,) == localStorage.getItem('userName')) {
            setIsMine(true);
        }
        document.addEventListener('mousedown', handleClickOutside);
        return () => {
          document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    return (
        <>
            <div className={styles.info}>
                <div className={styles.banner_photo_div}></div>
                <div className={styles.head}>
                    <div className={styles.user_info}>
                        <div className={styles.user_photo_wrapper}>
                            {user.photo == "null" && <div style={{filter: uploading && "grayscale(1)"}} onClick={() => {setPhotoList(!photoList)}} className={styles.default_user_photo}>
                                <div className={styles.camera}></div>
                                {uploading && <div className={styles.spinner}>
                                        <Spinner width={100} height={100} border={7} color={"black"} />
                                </div>}
                            </div>}
                            {user.photo != "null" && <>
                                <div onClick={() => {setPhotoList(!photoList)}} className={styles.wrapper}>
                                    <img style={{borderColor: `${user.stories.$values.length > 0 ? "#FFC737" : "white"}`}} className={styles.profile_photo} src={user.photo} alt="" />
                                    {uploading && <div className={styles.spinner}>
                                        <Spinner width={100} height={100} border={7}/>
                                    </div>}
                                    <div className={styles.camera}></div>
                                </div>
                            </>}
                            {photoList && <div ref={list}><ProfilePhotoList stories={user.stories.$values.length > 0} isMine={isMine} handleUpload={handleUpload} /></div>}
                        </div>
                        <div className={styles.user_info_details}>
                            <h1>{user.firstName} {user.lastName}</h1>
                            <div style={{display: "flex", alignItems: "center"}}>
                                <div className={styles.location}></div>
                                <p>Cairo, Egypt</p>
                            </div>
                            <div className={styles.friends}>
                                <p>300 friends</p>
                                <div className={styles.friends_photos}>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    {isLogged && <div className={styles.user_settings}>
                        <button>Add Friend</button>
                        {isMine && <button>Edit Profile Info</button>}
                    </div>}
                </div>
                <div className={styles.line}></div>
                <div className={styles.about}>
                    <h4>About</h4>
                    {editBio == false ? <>{user.about ? <p>{bio}</p> : <p style={{color: "gray", fontSize: "10pt"}}>{isMine ? <p>Tell the world your story !</p> : <p>user doesn't have bio</p>}</p>}</> : <><div className={styles.bio_textarea}>
                        <Image src={"/assets/smile.svg"}
                        onClick={() => {setEmojiPicker(!emojiPicker)}}
                        style={{position: "absolute", bottom: "10px", right: "10px", cursor: "pointer"}}
                        width={30} height={30}/>
                        <textarea value={editedBio} onChange={handleTextArea}/>
                        </div>
                        <EmojiPicker open={emojiPicker} style={{width: "100%"}} theme="light" onEmojiClick={handleEmojiClick}/></>
                        }
                    {editBio == false ? <>
                        {isMine && <button onClick={() => {setEditBio(true)}}>Edit about</button>}
                    </> : <div style={{display: "flex", alignItems: "center", marginLeft: "auto"}}>
                        <button className={styles.cancel_bio} onClick={() => {
                            setEditBio(false);
                            setEmojiPicker(false);
                        }}>cancel</button>
                        <button className={styles.save_bio} style={{marginLeft: "10px"}} onClick={() => {
                            setEditBio(false);
                            setBio(editedBio);
                            setEmojiPicker(false);
                            handleUpdateAbout(editedBio);
                        }}>save</button>
                    </div>}
                </div>
            </div>
        </>
    )
}
