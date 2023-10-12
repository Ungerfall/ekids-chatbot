package org.braidsencurls.ekids_homework_checker;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyDescription;

import java.util.List;

public class EvaluationResponse {
    public EvaluationResponse(boolean success, String criteria, List<String> sprite) {
        this.success = success;
        this.criteria = criteria;
        this.sprite = sprite;
    }
    public EvaluationResponse() {}

    @JsonPropertyDescription("boolean - flag if the criteria is met")
    @JsonProperty(required = true)
    private boolean success;
    @JsonPropertyDescription("string - criteria description")
    @JsonProperty(required = true)
    private String criteria;
    @JsonPropertyDescription("list of string - name of the sprites that met the criteria")
    @JsonProperty(required = true)
    private List<String> sprite;

    public boolean isSuccess() {
        return success;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }

    public String getCriteria() {
        return criteria;
    }

    public void setCriteria(String criteria) {
        this.criteria = criteria;
    }

    public List<String> getSprite() {
        return sprite;
    }

    public void setSprite(List<String> sprite) {
        this.sprite = sprite;
    }
}
