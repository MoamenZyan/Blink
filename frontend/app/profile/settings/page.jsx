"use client";
import Header from "@/components/header/header.module";
import styles from "./page.module.css";
import Image from "next/image";
import GetUserByUsername from "@/ApiHelper/users/getUserByUsername";
import StoryPrivacy from "@/components/storyPrivacy/storyPrivacy.module";
import { useState, useEffect, useRef } from "react";
import ProfileSettingsPopup from "@/components/profileSettingsPopups/popup.module";
import UpdateUserInfo from "@/ApiHelper/users/updateUserInfo";
import UploadUserPhoto from "@/ApiHelper/users/uploadUserPhoto";
import Spinner from "@/components/spinner/spinner.module";


export default function SettingsPage() {
    const input = useRef(null);
    const [profilePhotoURL, setProfilePhotoURL] = useState(null);
    const [user, setUser] = useState(null);
    const [privacyPopup, setPrivacyPopup] = useState(false);
    const [aboutPopup, setAboutPopup] = useState(false);
    const [namePopup, setNamePopup] = useState(false);
    const [locationPopup, setLocationPopup] = useState(false);
    const [overlay, setOverlay] = useState(false);
    const [privacy, setPrivacy] = useState(null);
    const [firstName, setFirstName] = useState(null);
    const [lastName, setLastName] = useState(null);
    const [about, setAbout] = useState(null);
    const [city, setCity] = useState(null);
    const [country, setCountry] = useState(null);
    const [photo, setPhoto] = useState(null);
    const [privacyIcon, setPrivacyIcon ] = useState(null);
    const [loading, setLoading] = useState(false);

    const handlePhoto = async () => {
        const photo = input.current.files[0];
        if (photo) {
            const form = new FormData();
            form.append('Photo', photo);
            setPhoto(photo);
            setProfilePhotoURL(URL.createObjectURL(photo));
            await UploadUserPhoto(form);
        }
    }

    useEffect(() => {
        const getInfo = async () => {
            const user = await GetUserByUsername(localStorage.getItem('userName'));
            handlePrivacyIcon(user.user.privacy);
            if (user) {
                setFirstName(user.user.firstName);
                setLastName(user.user.lastName);
                setPrivacy(user.user.privacy);
                setAbout(user.user.about);
                setPhoto(user.user.photo);
                setProfilePhotoURL(user.user.photo);
                setCity(user.user.city);
                setCountry(user.user.country);
                setUser(user.user);
            }
        }
        getInfo();
    }, []);

    const handleUpdateUserInfo = async () => {
        const form = new FormData();
        form.append('firstName', firstName);
        form.append('lastName', lastName);
        form.append('about', about);
        form.append('privacy', privacy);
        form.append('city', city);
        form.append('country', country);
        setLoading(true);
        await UpdateUserInfo(form);
        setLoading(false);
    }

    const handlePrivacyIcon = (privacy) => {
        switch (privacy) {
            case 'public':
                setPrivacyIcon("/assets/public_white.svg");
                break;
            case 'friends':
                setPrivacyIcon("/assets/friends_white.svg");
                break;
            case 'private':
                setPrivacyIcon("/assets/private_white.svg");
                break;
        }
    }

    if (user) {
        return (
            <>
                <Header overlay={overlay} />
                <div className={styles.parent}>
                    <h1>Profile Settings</h1>
                    <div className={styles.profile_photo}>
                        <img src={profilePhotoURL} alt="user_photo"/>
                        <input onChange={handlePhoto} ref={input} type="file" name="" id="" />
                        <span onClick={() => {input.current.click()}}>Edit</span>
                    </div>
                    <div className={styles.user_info}>
                        <div className={styles.info}>
                            <p>Name:</p>
                            <h3>{firstName} {lastName}</h3>
                            <div onClick={() => {
                                setNamePopup(true)
                                setOverlay(true);
                                }} className={styles.edit}>Edit</div>
                        </div>
                        <div className={styles.info}>
                            <p>I'am from:</p>
                            <p className={styles.about}>{city + ", " + country || "tell the world your story !"}</p>
                            <div onClick={() => {
                                setLocationPopup(true)
                                setOverlay(true);
                                }} className={styles.edit}>Edit</div>
                        </div>
                        <div className={styles.info}>
                            <p>About me:</p>
                            <p className={styles.about}>{about || "tell the world your story !"}</p>
                            <div onClick={() => {
                                setAboutPopup(true);
                                setOverlay(true);
                                }} className={styles.edit}>Edit</div>
                        </div>
                        <div onClick={() => {
                            setPrivacyPopup(true);
                            setOverlay(true);
                            }} className={styles.privacy}>
                            <p>Account Privacy:</p>
                            <div className={styles.wrapper}>
                                <div>
                                    <Image src={privacyIcon} alt="" width={25} height={25}/>
                                    <p>{user.privacy}</p>
                                </div>
                            </div>
                        </div>
                        <div onClick={!loading && handleUpdateUserInfo} style={{cursor: !loading ? "pointer" : "default"}} className={styles.save}>
                            {loading && <Spinner width={20} height={20} color={"white"} border={4} />}
                            {!loading && "SAVE"}
                        </div>
                    </div>
                    {privacyPopup &&
                        <StoryPrivacy
                        setPrivacyPopup={setPrivacyPopup}
                        setPrivacy={setPrivacy}
                        handleIcon={handlePrivacyIcon}
                        setOverlay={setOverlay}
                        text={"profile"}
                        />
                    }
                    {overlay && <div className={styles.overlay}></div>}

                    {aboutPopup && <ProfileSettingsPopup 
                        text={"About"}
                        type="about"
                        setInfo={setAbout}
                        setPopup={setAboutPopup}
                        setOverlay={setOverlay} />}

                    {namePopup && <ProfileSettingsPopup
                        text={"Name"}
                        type="name"
                        setInfo={[setFirstName, setLastName]}
                        setPopup={setNamePopup}
                        setOverlay={setOverlay} />}
    
                    {locationPopup && <ProfileSettingsPopup
                        text={"Location"}
                        type="location"
                        setInfo={[setCity, setCountry]}
                        setPopup={setLocationPopup}
                        setOverlay={setOverlay} />}
                </div>
            </>
        );
    }
}
