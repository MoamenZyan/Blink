// Get all comment replies
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllCommentRepliesAPI(id) {
    return await fetch(`${baseURL}/comment/${id}/replies`, {method: "GET"})
    .then(res => res.json())
    .then(data => {return data});
}
