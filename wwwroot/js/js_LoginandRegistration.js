function showForm(formId) {
            document.querySelectorAll('.form').forEach(form => form.classList.remove('active'));
            document.querySelectorAll('.tab').forEach(tab => tab.classList.remove('active'));
            document.getElementById(formId + 'Form').classList.add('active');
            event.target.classList.add('active');
        }

        document.getElementById('loginForm').addEventListener('submit', function(e) {
            e.preventDefault();
            const email = document.getElementById('loginEmail').value;
            const password = document.getElementById('loginPassword').value;
            console.log('Login:', { email, password });
          //LOGIN
        });

        document.getElementById('registerForm').addEventListener('submit', function(e) {
            e.preventDefault();
            const username = document.getElementById('regUsername').value;
            const email = document.getElementById('regEmail').value;
            const city = document.getElementById('regCity').value;
            const password = document.getElementById('regPassword').value;
            console.log('Register:', { username, email, city, password });
            //RGISTRATION
        });