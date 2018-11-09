package regexReplace;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {

    public static void main(String[] args) {
	    ArrayList<String> input = new ArrayList<String>();
	    input.add("(?<!por10\\\\s+)(?<!por10\\\\s*)(cien(to)?))");
        input.add("(?<!por10\\\\s*)(cien(to)?))");
        input.add("(?<!%|\\d)");
        System.out.printf("number of sentences: %d\n", input.size());
        replace_2(input);
    }

    public static void replace_1(ArrayList<String> textlist){
        Map<String, String> replace = new HashMap();
        replace.put("+", "{1, 10}");
        replace.put("*", "{0, 10}");
        Pattern pattern = Pattern.compile("(?<=(\\?<[!=][^)]{1,100}))(\\+|\\*)");
        for(String text: textlist){
            Matcher matcher = pattern.matcher(text);
            if(matcher.find()){
                text = matcher.replaceAll(replace.get(matcher.group(2)));
            }
            System.out.printf("after replace: %s\n", text);
        }
    }

    public static void replace_2(ArrayList<String> textlist){
        Map<String, String> replace = new HashMap();
        replace.put("+", "{1, 10}");
        replace.put("*", "{0, 10}");
        Pattern pattern = Pattern.compile("(\\?<[!=][^)]+)(\\+|\\*)");
        for(String text: textlist){
            Matcher matcher = pattern.matcher(text);
            if(matcher.find()){
                text = matcher.replaceAll(matcher.group(1)+replace.get(matcher.group(2)));
            }
            System.out.printf("after replace: %s\n", text);
        }
    }
}
