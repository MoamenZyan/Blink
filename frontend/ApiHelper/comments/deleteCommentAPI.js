// Delete comment
const baseURL = "http://localhost:8080/api/v1";
export default async function DeleteCommentAPI(id) {
    return await fetch(`${baseURL}/comment/${id}`, {method: "DELETE", credentials: "include"})
    .then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    });
}
