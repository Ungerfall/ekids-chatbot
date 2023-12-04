FROM openjdk:17-oracle
COPY target/homework-checker*.jar /usr/app/homework-checker.jar
WORKDIR /usr/app
ENTRYPOINT ["java", "-jar", "homework-checker.jar"]
EXPOSE 8080
