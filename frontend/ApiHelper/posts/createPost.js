// Create Post
const baseURL = "http://localhost:8080/api/v1";
export default async function CreatePost(form) {
    return await fetch(`${baseURL}/posts`, {
        method: "POST",
        credentials: "include",
        body: form
    })
    .then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    })
}
