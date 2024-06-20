// Gets all stories from all users
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllStories() {
    return await fetch(`${baseURL}/all-stories`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
