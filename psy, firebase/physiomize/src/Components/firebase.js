import { initializeApp } from "firebase/app";

const firebaseConfig = {
  apiKey: "AIzaSyBu_FilKyKEnTcLRjF8qnNNMOWwD72Q4bw",
  authDomain: "projectpsy-815fa.firebaseapp.com",
  databaseURL:
    "https://projectpsy-815fa-default-rtdb.asia-southeast1.firebasedatabase.app",
  projectId: "projectpsy-815fa",
  storageBucket: "projectpsy-815fa.appspot.com",
  messagingSenderId: "993312168850",
  appId: "1:993312168850:web:df9493f81668c26ddabf4b",
};

const firebaseApp = initializeApp(firebaseConfig);

export default firebaseApp;
