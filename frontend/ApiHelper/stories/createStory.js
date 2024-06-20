// Create Story
const baseURL = "http://localhost:8080/api/v1";
export default async function CreateStory(form) {
    return await fetch(`${baseURL}/stories`, {
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
    });
}
