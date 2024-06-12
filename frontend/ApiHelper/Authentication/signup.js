
// Signup api
const baseURL = "http://localhost:8080/api/v1";
export default function Signup(data) {
    const formData = new URLSearchParams(data);
    return fetch(`${baseURL}/users`, {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
        },
        body: formData
    }).then(res => {
        if (res.ok)
            return true;
        else
            return false;
    })
}
