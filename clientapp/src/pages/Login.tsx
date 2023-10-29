import { useState } from 'react';
import './Login.css';
import { useCookies } from 'react-cookie';

function LoginPage() {
	const list_cookeies = ['jwt_token'];
	const [cookies, setCookie, removeCookie] = useCookies(list_cookeies);

	const [loginField, setLoginField] = useState('');
	const [passwordField, setPasswordField] = useState('');

	const handleSetCookie = (name: string, value: string) => {
		setCookie(name, value, { path: '/' });
	};

	const handleRemoveCookie = (name: string) => {
		removeCookie(name);
	};

	const handleLoginChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setLoginField(event.target.value);
	};

	const handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setPasswordField(event.target.value);
	};


	async function buttonSingInClick() {
		const response = await fetch("https://localhost:7025/api/v1/SignIn", {
			method: "POST",
			headers: { "Accept": "application/json", "Content-Type": "application/json" },
			body: JSON.stringify({
				login: loginField,
				password: passwordField
			})
		});

		if (response.ok) {
			const data = await response.json();
			handleSetCookie("jwt_token", data.access_token);
			console.log(data.access_token);
		}
	}

	async function buttonSignOutClick() {
		handleRemoveCookie("jwt_token")
	}

	return (
		<div className='Container'>
			<div className='ContainerHori'>
				<label style={{ width: '100px' }}>Login:</label>
				<input style={{ margin: '0 5px' }}
					type="text"
					value={loginField}
					onChange={handleLoginChange} />
			</div>

			<div className='ContainerHori'>
				<label style={{ width: '100px' }}>Password:</label>
				<input style={{ margin: '0 5px' }}
					type="password"
					value={passwordField}
					onChange={handlePasswordChange} />
			</div>

			<button style={{ width: '150px', margin: '10px 10px 0 0' }} onClick={buttonSingInClick}>SignIn</button>
			<button style={{ width: '150px', margin: '10px 10px 0 0' }} onClick={buttonSignOutClick}>SignOut</button>
		</div>
	);
}

export default LoginPage;
