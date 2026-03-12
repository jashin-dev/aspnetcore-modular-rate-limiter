import http from 'k6/http';
import exec from 'k6/execution';

export const options = {
  vus: 10,
  iterations: 10,
};

export default function () {
  const res = http.get('http://localhost:5000/api/test');

  console.log(
    `VU ${exec.vu.idInTest} Iteration ${exec.vu.iterationInInstance} -> Status ${res.status} RetryAfter: ${res.headers['Retry-After']}`
  );
}