
// Login api
const baseURL = "http://localhost:8080/api/v1";
export default async function Login(data) {
    const formData = new URLSearchParams(data);
    return await fetch(`${baseURL}/login`, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
        },
        body: formData
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
