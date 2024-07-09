"use client";
import styles from "./browseStory.module.css";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Image from "next/image";
import { formatDistanceToNow } from 'date-fns';

export default function BrowseStory({story, length, progress, currentIndex, navigation, prev, next, isPaused, setIsPaused}) {
    const router = useRouter();

    if (story) {
        return (
            <>
                <div style={{backgroundColor: `${story.backgroundColor}`}} className={styles.story_div}>
                    {prev && <div onClick={() => {navigation("prev")}} className={styles.arrow_left}>
                        <Image src={"/assets/arrow_left.svg"} width={30} height={30} alt=""/>
                    </div>}
                    <div onClick={() => {setIsPaused(!isPaused)}} className={styles.body}>
                        <div className={styles.story_lines}>
                            {Array.from({length}).map((_, index) => (
                                <div className={styles.line}>
                                    <span style={{width: `${currentIndex == index && progress || 0}%`}} className={styles.progress}></span>
                                </div>
                            ))}
                        </div>
                        <div className={styles.info}>
                            <div onClick={() => {router.push(`/profile/${story.username}`)}} className={styles.user_info}>
                                <img src={story.userPhoto} alt=""/>
                                <div className={styles.info_details}>
                                    <h4>{story.username}</h4>
                                    <p>{formatDistanceToNow(new Date(story.createdAt), {addSuffix: true})}</p>
                                </div>
                            </div>
                            <div className={styles.pause_wrapper}>
                                <Image className={styles.pause} src={`/assets/${isPaused ? "resume" : "pause"}.svg`} width={50} height={50} alt=""/>
                            </div>
                        </div>
                        {story.photo != "null" && <img className={styles.story_photo} src={story.photo} alt=""/>}
                        <div style={{color: `${story.textColor}`}} className={styles.text}>{story.content}</div>
                    </div>
                    {next && <div onClick={() => {navigation("next")}} className={styles.arrow_right}>
                        <Image src={"/assets/arrow_right.svg"} width={30} height={30} alt=""/>
                    </div>}
                </div>
            </>
        );
    }
}
