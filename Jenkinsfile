pipeline{
    agent any
    stages {
        stage("Deliver database") {
            steps {
                sh "docker-compose up -d"
            }
        }
    }
}