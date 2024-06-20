// give like to a post
const baseURL = "http://localhost:8080/api/v1";
export default async function DeleteReactionFromAPost(id) {
    return await fetch (`${baseURL}/reaction-post/${id}`, {
        method: "DELETE",
        credentials: "include",
    })
    .then((res) => {
        if (res.ok)
            return true;
        else
            return false;
    });
}