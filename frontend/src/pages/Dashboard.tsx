import api from '../api/api';

export default function Dashboard() {
  const getWorkouts = async () => {
    const res = await api.get('api/workoutsapi');
    console.log(res.data);
  };

  return (
    <div>
      <p>Logged into dashboard</p>
      <button onClick={getWorkouts}>fetch workout data</button>
    </div>
  );
}
