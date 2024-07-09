// Delete reply
const baseURL = "http://localhost:8080/api/v1";
export default async function DeleteReply(id) {
    return await fetch(`${baseURL}/reply/${id}`, {method: "DELETE", credentials: "include"})
    .then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    });
}
