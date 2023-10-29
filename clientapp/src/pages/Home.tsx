import './Home.css';
import { useCookies } from 'react-cookie';

function HomePage() {
	const list_cookeies = ['jwt_token'];
	const [cookies] = useCookies(list_cookeies);

	async function buttonGetSourceClick() {
		const response = await fetch("https://localhost:7025/api/v1/Index", {
			method: "GET",
			headers: {
				"Accept": "application/json",
				"Authorization": "Bearer " + cookies["jwt_token"]
			}
		});

		if (response.ok) {
			console.log(await response.json())
		}
	}

	return (
		<div className='Container'>
			<button style={{ width: '150px', margin: '10px 10px 0 0' }} onClick={buttonGetSourceClick}>GetSource</button>
		</div>
	);
}


export default HomePage;
