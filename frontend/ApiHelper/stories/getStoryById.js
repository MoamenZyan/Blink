// Gets story with it's id

const baseURL = "http://localhost:8080/api/v1";
export default async function GetStoryById(id) {
    return await fetch(`${baseURL}/story/${id}`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
