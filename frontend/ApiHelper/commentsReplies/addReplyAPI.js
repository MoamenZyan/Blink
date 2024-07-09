// Create Reply
const baseURL = "http://localhost:8080/api/v1";
export default async function CreateReplyOnACommentAPI(form) {
    const formData = new URLSearchParams(form);
    return await fetch(`${baseURL}/replies`, {
        method: "POST",
        credentials: "include",
        body: formData
    })
    .then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    })
}
