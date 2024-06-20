
// Get all users posts
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllPosts() {
    return await fetch(`${baseURL}/posts`, {
        method: "GET",
    })
    .then(res => res.json())
    .then((data) => data);
}
