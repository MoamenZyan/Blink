
// Upload Profile Banner
const baseURL = "http://localhost:8080/api/v1";
export default async function UploadUserBanner(form) {
    return await fetch(`${baseURL}/user-banner`, {
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
