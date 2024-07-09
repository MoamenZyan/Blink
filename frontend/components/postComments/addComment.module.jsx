"use client";
import styles from "./addComment.module.css"
import Image from "next/image";
import CreateComment from "@/ApiHelper/comments/createCommentAPI";
import CreateReplyOnACommentAPI from "@/ApiHelper/commentsReplies/addReplyAPI";
import { useState, useRef, useEffect } from "react";
import EmojiPicker from "emoji-picker-react";

export default function AddComment({postId, setTrigger, trigger, setCommentsCount, type, commentId}) {
    const [content, setContent] = useState(null);
    const [picker, setPicker] = useState(false);
    const ref = useRef(null);
    const pickerRef = useRef(null);
    const input = useRef(null);

    const handleCreateComment = async () => {
        const form = new FormData();
        form.append('PostId', Number(postId));
        form.append('Content', content);
        await CreateComment(form);
        setTrigger(!trigger);
        setCommentsCount(count => count + 1);
        setContent("");
    }

    const handleCreateReply = async () => {
        const form = new FormData();
        form.append('PostId', Number(postId));
        form.append('CommentId', Number(commentId));
        form.append('Content', content);
        await CreateReplyOnACommentAPI(form);
        setTrigger(!trigger);
        setCommentsCount(count => count + 1);
        setContent("");
    }

    const handleClickOutside = (event) => {
        if (ref.current && pickerRef.current&& 
            !ref.current.contains(event.target)
            && !pickerRef.current.contains(event.target)) {
            setPicker(false);
        }
    }

    useEffect(() => {
        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        }
    }, []);

    return (
    <>
        <div className={styles.parent}>
            <div className={styles.left}>
                <input value={content} onChange={(e) => {setContent(e.target.value)}} placeholder={type == "reply" ? "write a reply..." : "write a comment..."}/>
            </div>
            <div className={styles.right}>
                <Image ref={ref} onClick={() => {setPicker(!picker)}} src={"/assets/emoji.svg"} width={20} height={20} alt=""/>
                <Image onClick={() => {input.current.click()}} src={"/assets/add_image.svg"} width={20} height={20} alt=""/>
                <Image onClick={type == "reply" ? handleCreateReply : handleCreateComment} src={"/assets/rocket.svg"} width={20} height={20} alt=""/>
            </div>
            { picker && <div ref={pickerRef} className={styles.emoji}><EmojiPicker onEmojiClick={(emoji) => {setContent(content => content + emoji.emoji)}} style={{position: "absolute", zIndex: "80", top: "50px"}} /></div>}
            <input ref={input} className={styles.input} type="file"/>
        </div>
    </>
    );
}