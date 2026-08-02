import { useState, type SubmitEvent } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../api/api';
import { useNavigate } from 'react-router';

interface LoginResponse {
  token: string;
}

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: SubmitEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      const res = await api.post<LoginResponse>('/api/auth/login', {
        email,
        password,
      });
      login(res.data.token);
      navigate('/', { replace: true });
    } catch {
      setError('Invalid email or password.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div>
      <h1>Log in</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        {error && <p role="alert">{error}</p>}

        <button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Logging in...' : 'Log in'}
        </button>
      </form>
    </div>
  );
}
