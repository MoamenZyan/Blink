// Create Comment
const baseURL = "http://localhost:8080/api/v1";
export default async function CreateComment(form) {
    const formData = new URLSearchParams(form);
    return await fetch(`${baseURL}/comments`, {
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
