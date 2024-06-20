
// Upload Profile Photo
const baseURL = "http://localhost:8080/api/v1";
export default async function UploadUserPhoto(form) {
    return await fetch(`${baseURL}/user-photo`, {
        method: "POST",
        credentials: "include",
        body: form
    }).then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    })
}
